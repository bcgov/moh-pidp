import { CommonModule } from '@angular/common';
import {
  Component,
  Inject,
  TemplateRef,
  ViewChild,
  inject,
  signal,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { EMPTY, catchError, tap } from 'rxjs';

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
export class ExternalAccountsPage {
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);

  public AccessRoutes = AccessRoutes;
  public componentType = DialogExternalAccountCreateComponent;
  public emailSupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private dialog: MatDialog,
    private loadingOverlay: LoadingOverlayService,
    private partyService: PartyService,
    private resource: ExternalAccountsResource,
    private router: Router,
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

  public currentStep = 1;
  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<Element>;

  public cards = signal<InstructionCard[]>([
    {
      id: 1,
      icon: 'instruction-document',
      title: 'Choose from the options',
      description:
        'Select from the list of accepted domains below and enter your complete username (e.g., joe@bchealthcompany.com).',
      type: 'dropdown',
      placeholder: 'Yukon',
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
    //TODO : to be removed.
    if (index === 0 || index === 2) {
      return false;
    }
    return index === this.currentStep;
  }

  public isCardCompleted(index: number): boolean {
    //TODO : to be removed.
    if (index === 0 || index === 2) {
      return false;
    }
    return index < this.currentStep;
  }

  public onContinue(index: number, value: string): void {
    // Handle the continue action for each step
    console.log(`Step ${index + 1} completed with value:`, value);

    if (index + 1 === InvitationSteps.USER_PRINCIPAL_NAME) {
      this.loadingOverlay.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
      this.resource
        .createExternalAccount(this.partyService.partyId, value)
        .pipe(
          tap(() => {
            this.loadingOverlay.close();
            this.toastService.openSuccessToast('Account invited successfully!');
          }),
          catchError(() => {
            this.loadingOverlay.close();
            this.toastService.openErrorToast('Failed to invite account.');
            return EMPTY;
          }),
        )
        .subscribe();
    }

    // Move to the next step if not the last one
    if (index < this.cards().length - 1) {
      this.currentStep = index + 2;
    } else {
      // Handle completion of all steps
      console.log('All steps completed!');
      this.router.navigate([value]);
      this.showSuccessDialog();
    }
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {};
    this.dialog.open(this.successDialogTemplate, config);
  }
}
