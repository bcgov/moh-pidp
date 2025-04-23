/* eslint-disable @typescript-eslint/no-explicit-any */
import { ViewContainerRef } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { provideAutoSpy } from 'jest-auto-spies';

import { DialogBcproviderCreateComponent } from './components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from './components/dialog-bcprovider-edit.component';
import { SuccessDialogComponent } from './success-dialog.component';

describe('SuccessDialogComponent', () => {
  let component: SuccessDialogComponent;
  let fixture: ComponentFixture<SuccessDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatDialogModule],
      providers: [SuccessDialogComponent, provideAutoSpy(MatDialog)],
    }).compileComponents();

    component = TestBed.inject(SuccessDialogComponent);
    fixture = TestBed.createComponent(SuccessDialogComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should close all dialogs when onSuccessDialogClose is called', () => {
    const dialogSpy = jest.spyOn(component.dialog, 'closeAll');

    component.onSuccessDialogClose();

    expect(dialogSpy).toHaveBeenCalled();
  });

  it('should set showHeader to true if componentType is DialogBcproviderCreateComponent or DialogBcproviderEditComponent', () => {
    component.componentType = DialogBcproviderCreateComponent;
    component.ngOnInit();
    expect(component.showHeader).toBe(true);

    component.componentType = DialogBcproviderEditComponent;
    component.ngOnInit();
    expect(component.showHeader).toBe(true);
  });

  it('should log an error if dialogParagraphHost is not initialized', () => {
    const consoleErrorSpy = jest.spyOn(console, 'error');
    component.dialogParagraphHost = undefined as unknown as ViewContainerRef;

    component.ngOnInit();

    expect(consoleErrorSpy).toHaveBeenCalledWith(
      'DialogParagraphHost is not initialized.',
    );
  });

  it('should create a component and set username if dialogParagraphHost is initialized', () => {
    const mockViewContainerRef = {
      createComponent: jest.fn().mockReturnValue({
        instance: {},
      }),
    } as unknown as ViewContainerRef;

    component.dialogParagraphHost = mockViewContainerRef;
    component.username = 'testUser';
    component.componentType = class {} as any;

    component.ngOnInit();

    expect(mockViewContainerRef.createComponent).toHaveBeenCalledWith(
      component.componentType,
    );
    expect(
      mockViewContainerRef.createComponent(component.componentType).instance
        .username,
    ).toBe('testUser');
    jest.clearAllMocks();
  });

  it('should log a warning if username is undefined', () => {
    const consoleWarnSpy = jest.spyOn(console, 'warn');
    const mockViewContainerRef = {
      createComponent: jest.fn().mockReturnValue({
        instance: {},
      }),
    } as unknown as ViewContainerRef;

    component.dialogParagraphHost = mockViewContainerRef;
    component.username = '';
    component.componentType = class {} as any;

    component.ngOnInit();

    expect(consoleWarnSpy).toHaveBeenCalledWith('Username is undefined.');
    jest.clearAllMocks();
  });
});
