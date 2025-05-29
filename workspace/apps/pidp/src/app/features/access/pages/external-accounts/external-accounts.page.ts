import { CommonModule } from '@angular/common';
import {
  Component,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
  computed,
  inject,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import {
  EMPTY,
  catchError,
  distinctUntilChanged,
  map,
  of,
  switchMap,
  tap,
} from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { ToastService } from '@app/core/services/toast.service';
import { DialogExternalAccountCreateComponent } from '@app/shared/components/success-dialog/components/external-account-create.component';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '../../access.routes';
import { InstructionCardComponent } from './components/instruction-card.component';
import { InstructionCard } from './components/instruction-card.model';
import { ExternalAccountsResource } from './external-accounts-resource.service';
import { InvitationSteps } from './external-accounts.model';

@Component({
  selector: 'app-external-accounts',
  standalone: true,
  imports: [
    CommonModule,
    BreadcrumbComponent,
    InstructionCardComponent,
    MatIconModule,
    InjectViewportCssClassDirective,
    SuccessDialogComponent,
    AnchorDirective,
  ],
  templateUrl: './external-accounts.page.html',
  styleUrl: './external-accounts.page.scss',
})
export class ExternalAccountsPage implements OnInit {
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);

  public AccessRoutes = AccessRoutes;
  public componentType = DialogExternalAccountCreateComponent;
  public emailSupport: string;
  public emailValidationToken = signal<string | null>(null);

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private dialog: MatDialog,
    private loadingOverlay: LoadingOverlayService,
    private partyService: PartyService,
    private resource: ExternalAccountsResource,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService,
  ) {
    this.registerSvgIcons();
    this.emailSupport = this.config.emails.providerIdentitySupport;
  }

  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'Halo', path: AccessRoutes.routePath(AccessRoutes.HALO) },
    { title: 'External Accounts', path: '' },
  ];

  public registerSvgIcons(): void {
    // Register each SVG icon with MatIconRegistry
    this.matIconRegistry.addSvgIcon(
      'instruction-document',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/instruction-document.svg',
      ),
    );
    this.matIconRegistry.addSvgIcon(
      'instruction-pencil',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/instruction-pencil.svg',
      ),
    );
    this.matIconRegistry.addSvgIcon(
      'instruction-mail',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/instruction-mail.svg',
      ),
    );
    this.matIconRegistry.addSvgIcon(
      'icon-complete',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/icon-complete.svg',
      ),
    );

    this.matIconRegistry.addSvgIcon(
      'need-assistance',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/need-assistance.svg',
      ),
    );
  }

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<Element>;

  public cards = signal<InstructionCard[]>([
    {
      id: 1,
      icon: 'instruction-document',
      title: 'Start your search here',
      description: 'Search from the list of accepted domains below.',
      type: 'dropdown',
      placeholder: 'Search for a domain or use keywords',
      options: [
        { label: 'Yukon', value: 'yukon' },
        { label: 'British Columbia', value: 'bc' },
        { label: 'Alberta', value: 'alberta' },
      ],
      linkText: "Don't see your organization?",
    },
    {
      id: 2,
      icon: 'instruction-pencil',
      title: 'Enter account based on your choice in step one',
      description: '',
      type: 'input',
      placeholder: 'username@example.com',
      buttonText: 'Continue',
    },
    {
      id: 3,
      icon: 'instruction-mail',
      title: 'Please verify your email address',
      placeholder: '',
      description:
        'Click the verified link in the email that was just sent to you.',
      type: 'verification',
    },
    {
      id: 4,
      icon: 'icon-complete',
      title: 'Instructions complete',
      placeholder: '',
      description:
        'Click the “Continue” button to start using your own account.',
      type: 'final',
      buttonText: 'Continue',
    },
  ]);

  public isCardActive(index: number): boolean {
    const step = this.resource.currentStep();
    // Enable both step 3 (index 2) and step 4 (index 3) together if step >= 2
    if (step >= 2) {
      return (
        index === InvitationSteps.EMAIL_VERIFICATION - 1 ||
        index === InvitationSteps.COMPLETED - 1
      );
    }

    return index === step;
  }

  public isCardCompleted(index: number): boolean {
    if (index === 0 || index === 2) {
      // Step 1 and Step 3 are not considered completed
      return false;
    }
    return index < this.resource.currentStep();
  }

  public onContinue(index: number, value: string): void {
    // Handle the continue action for each step
    console.log(`Step ${index + 1} completed with value:`, value);
    this.cards().forEach((card) => {
      if (card.id === InvitationSteps.USER_PRINCIPAL_NAME) {
        card.placeholder = value;
      }
    });

    if (index + 1 === InvitationSteps.COMPLETED) {
      // Handle completion of all steps
      console.log('All steps completed!');
      this.resource.currentStep.set(0);
      this.router.navigate([value]);
      this.showSuccessDialog();
    }

    if (index + 1 === InvitationSteps.USER_PRINCIPAL_NAME) {
      // TODO: Remove this and get the email from verify endpoint
      localStorage.setItem('userPrincipalName', value);
      this.loadingOverlay.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
      this.resource
        .checkIfEmailIsVerified(this.partyService.partyId, value)
        .pipe(
          tap(() => {
            this.loadingOverlay.close();
            this.toastService.openSuccessToast('Account checked successfully!');

            // Move to the next step if not the last one
            if (index < this.cards().length - 1) {
              this.resource.currentStep.set(index + 1);
            }
          }),
          catchError(() => {
            this.loadingOverlay.close();
            this.toastService.openErrorToast('Failed to invite account.');
            return EMPTY;
          }),
        )
        .subscribe();
    } else {
      this.resource.currentStep.set(index + 1);
    }
  }

  public ngOnInit(): void {
    this.loadingOverlay.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.route.queryParamMap
      .pipe(
        map((params) => params.get('email-verification-token')),
        distinctUntilChanged(),
        tap((token) => {
          console.log('Email verification token:', token);
          this.emailValidationToken.set(token);
        }),
        switchMap((token) =>
          token
            ? this.resource.verifyEmail(this.partyService.partyId, token).pipe(
                switchMap(() =>
                  this.resource
                    .createExternalAccount(
                      this.partyService.partyId,
                      localStorage.getItem('userPrincipalName') || '',
                    )
                    .pipe(
                      tap(() => {
                        this.loadingOverlay.close();
                        this.toastService.openSuccessToast(
                          'External Account invited successfully!',
                        );
                      }),
                    ),
                ),
              )
            : // If no token, return an observable that emits null
              of(null),
        ),
      )
      .subscribe({
        next: (res) => {
          console.log('Email verification response:', res);
          if (res !== null) {
            this.resource.currentStep.set(3);
          }
        },
        error: (error) => {
          console.error('Error during email verification:', error);
        },
        complete: () => {
          console.log('Email verification process completed.');
        },
      });
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {};
    this.dialog.open(this.successDialogTemplate, config);
  }
}
