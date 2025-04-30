import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '../../access.routes';
import { InstructionCardComponent } from './components/instruction-card.component';

@Component({
  selector: 'app-external-accounts',
  standalone: true,
  imports: [
    CommonModule,
    BreadcrumbComponent,
    InstructionCardComponent,
    MatIconModule,
    InjectViewportCssClassDirective,
  ],
  templateUrl: './external-accounts.page.html',
  styleUrl: './external-accounts.page.scss',
})
export class ExternalAccountsPage {
  public sanitizer = inject(DomSanitizer);
  public matIconRegistry = inject(MatIconRegistry);

  public constructor() {
    this.registerSvgIcons();
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
      'instruction-time',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/instruction-time.svg',
      ),
    );

    this.matIconRegistry.addSvgIcon(
      'need-assistance',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/need-assistance.svg',
      ),
    );
  }

  public currentStep = 0;

  public cards = signal([
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
      description:
        'Click the verified link in the email that was just sent to you.',
      type: 'verification',
    },
    {
      id: 4,
      icon: 'instruction-time',
      title: 'Instructions complete',
      description:
        'Click the “Continue” button to start using your own account.',
      type: 'final',
      buttonText: 'Continue',
    },
  ]);

  public isCardActive(index: number): boolean {
    return index === this.currentStep;
  }

  public isCardCompleted(index: number): boolean {
    return index < this.currentStep;
  }

  public onContinue(index: number, value: any): void {
    // Handle the continue action for each step
    console.log(`Step ${index + 1} completed with value:`, value);

    // Move to the next step if not the last one
    if (index < this.cards().length - 1) {
      this.currentStep = index + 1;
    } else {
      // Handle completion of all steps
      console.log('All steps completed!');
    }
  }
}
