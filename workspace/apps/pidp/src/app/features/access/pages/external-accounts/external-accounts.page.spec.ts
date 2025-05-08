/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { NO_ERRORS_SCHEMA, TemplateRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { MatDialog } from '@angular/material/dialog';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG } from '@app/app.config';

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
        provideAutoSpy(Router),
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

  describe('isCardActive', () => {
    it('should return false for index 0 and 2', () => {
      expect(component.isCardActive(0)).toBe(false);
      expect(component.isCardActive(2)).toBe(false);
    });
    it('should return true if index equals currentStep', () => {
      component.currentStep = 1;
      expect(component.isCardActive(1)).toBe(true);
    });
    it('should return false if index does not equal currentStep', () => {
      component.currentStep = 2;
      expect(component.isCardActive(1)).toBe(false);
    });
  });

  describe('isCardCompleted', () => {
    it('should return false for index 0 and 2', () => {
      expect(component.isCardCompleted(0)).toBe(false);
      expect(component.isCardCompleted(2)).toBe(false);
    });
    it('should return true if index < currentStep', () => {
      component.currentStep = 3;
      expect(component.isCardCompleted(1)).toBe(true);
    });
    it('should return false if index >= currentStep', () => {
      component.currentStep = 1;
      expect(component.isCardCompleted(1)).toBe(false);
    });
  });

  describe('onContinue', () => {
    it('should advance currentStep if not last card', () => {
      const initialStep = 2;
      component.currentStep = initialStep;
      jest.spyOn(component as any, 'showSuccessDialog');
      component.onContinue(2, 'testValue');
      expect(component.currentStep).toBe(2);
      expect(router.navigate).not.toHaveBeenCalled();
      expect((component as any).showSuccessDialog).not.toHaveBeenCalled();
    });

    it('should call router.navigate and showSuccessDialog if last card', () => {
      component.currentStep = 3;
      jest.spyOn(component as any, 'showSuccessDialog');
      const cardsLength = component.cards().length;
      component.onContinue(cardsLength - 1, '/home');
      expect(router.navigate).toHaveBeenCalledWith(['/home']);
      expect((component as any).showSuccessDialog).toHaveBeenCalled();
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
