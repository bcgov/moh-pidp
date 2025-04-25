import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '../../access.routes';
import { InstructionCardComponent } from './components/instruction-card.component';

@Component({
  selector: 'app-external-accounts',
  standalone: true,
  imports: [CommonModule, BreadcrumbComponent, InstructionCardComponent],
  templateUrl: './external-accounts.page.html',
  styleUrl: './external-accounts.page.scss',
})
export class ExternalAccountsPage {
  constructor(
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer,
  ) {
    this.registerSvgIcons();
  }
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'PAS', path: AccessRoutes.routePath(AccessRoutes.HALO) },
    { title: 'External Accounts', path: '' },
  ];

  registerSvgIcons(): void {
    // Register each SVG icon with MatIconRegistry
    this.iconRegistry.addSvgIcon(
      'instruction-document',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/instruction-document.svg',
      ),
    );
    this.iconRegistry.addSvgIcon(
      'instruction-pencil',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        './assets/images/icons/instruction-pencil.svg',
      ),
    );
    this.iconRegistry.addSvgIcon(
      'instruction-mail',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/instruction-mail.svg',
      ),
    );
    this.iconRegistry.addSvgIcon(
      'instruction-time',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/instruction-time.svg',
      ),
    );
    this.iconRegistry.addSvgIcon(
      'chevron-down',
      this.sanitizer.bypassSecurityTrustResourceUrl(
        'assets/images/icons/chevron-down.svg',
      ),
    );
  }

  instructions = [
    {
      id: 1,
      title: 'Choose from the options',
      description:
        'Select from the list of accepted domains below and enter your complete username (e.g., joe@bchealthcompany.com).',
      icon: 'instruction-document',
      content: {
        type: 'dropdown',
        placeholder: 'Yukon',
        helpText: "Don't see your organization?",
      },
      isCompleted: false,
      isActive: true,
    },
    {
      id: 2,
      title: 'Enter account based on your choice in step one',
      description: '',
      icon: 'instruction-pencil',
      content: {
        type: 'input',
        placeholder: 'username@example.com',
        buttonText: 'Continue',
      },
      isCompleted: false,
      isActive: true,
    },
    {
      id: 3,
      title: 'Please verify your email address',
      description:
        'Click the verified link in the email that was just sent to you.',
      icon: 'instruction-mail',
      content: {
        type: 'message',
      },
      isCompleted: false,
      isActive: true,
    },
    {
      id: 4,
      title: 'Final setup from OneHealthID',
      description:
        'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum pulvinar turpis lorem',
      icon: 'instruction-time',
      content: {
        type: 'message',
        buttonText: 'Continue',
      },
      isCompleted: false,
      isActive: false,
    },
  ];
}
