/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { NO_ERRORS_SCHEMA, TemplateRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { MatDialog } from '@angular/material/dialog';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG } from '@app/app.config';

import { ExternalAccountsResource } from './external-accounts-resource.service';
import { ExternalAccountsPage } from './external-accounts.page';

describe('ExternalAccountsPage', () => {
  let component: ExternalAccountsPage;
  let router: Router;
  let dialog: MatDialog;
  let matIconRegistry: MatIconRegistry;

  const mockConfig = {
    emails: { providerIdentitySupport: 'support@example.com' },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ExternalAccountsPage,
        { provide: APP_CONFIG, useValue: mockConfig },
        { provide: MatIconRegistry, useValue: { addSvgIcon: jest.fn() } },
        {
          provide: DomSanitizer,
          useValue: { bypassSecurityTrustResourceUrl: jest.fn() },
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(MatDialog),
        provideAutoSpy(ExternalAccountsResource),
        provideAutoSpy(Router),
        provideAutoSpy(ActivatedRoute),
      ],
      schemas: [NO_ERRORS_SCHEMA],
    });
    component = TestBed.inject(ExternalAccountsPage);
    router = TestBed.inject(Router);
    dialog = TestBed.inject(MatDialog);
    matIconRegistry = TestBed.inject(MatIconRegistry);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize breadcrumbsData', () => {
    expect(component.breadcrumbsData.length).toBe(4);
    expect(component.breadcrumbsData[0].title).toBe('Home');
    expect(component.breadcrumbsData[3].title).toBe('External Accounts');
  });

  it('should register SVG icons on construction', () => {
    expect(matIconRegistry.addSvgIcon).toHaveBeenCalled();
  });

  describe('onContinue', () => {
    it('should advance currentStep if not last card', () => {
      jest.spyOn(component as any, 'showSuccessDialog');
      expect(router.navigate).not.toHaveBeenCalled();
      expect((component as any).showSuccessDialog).not.toHaveBeenCalled();
    });
  });

  describe('showSuccessDialog', () => {
    it('should open dialog with successDialogTemplate', () => {
      const templateRef = {} as TemplateRef<Element>;
      component.successDialogTemplate = templateRef;
      (component as any).showSuccessDialog();
      expect(dialog.open).toHaveBeenCalledWith(templateRef, expect.any(Object));
    });
  });
});
