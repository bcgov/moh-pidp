import { Injectable } from '@angular/core';
import {
  MatSnackBar,
  MatSnackBarConfig,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private readonly defaultConfig: MatSnackBarConfig;
  private readonly durationInMilliSecs: number;
  private readonly horizontalPosition: MatSnackBarHorizontalPosition;
  private readonly verticalPosition: MatSnackBarVerticalPosition;

  public constructor(private snackBar: MatSnackBar) {
    this.durationInMilliSecs = 3000;
    this.verticalPosition = 'bottom';
    this.horizontalPosition = 'center';
    this.defaultConfig = {
      duration: this.durationInMilliSecs,
      horizontalPosition: this.horizontalPosition,
      verticalPosition: this.verticalPosition,
    };
  }

  /**
   * @description
   * Opens a toast to display an information message.
   */
  public openSuccessToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig,
  ): void {
    this.openToast(message, action, {
      ...this.defaultConfig,
      panelClass: ['success'],
      ...config,
    });
  }

  /**
   * @description
   * Opens a toast to display an error message.
   */
  public openInfoToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig,
  ): void {
    this.openToast(message, action, {
      ...this.defaultConfig,
      panelClass: ['info'],
      ...config,
    });
  }

  /**
   * @description
   * Opens a toast to display an error message.
   */
  public openErrorToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig,
  ): void {
    this.openToast(message, action, {
      ...this.defaultConfig,
      panelClass: ['danger'],
      ...config,
    });
  }

  private openToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig,
  ): void {
    this.snackBar.open(message, action, config);
  }

  public closeToast(): void {
    this.snackBar.dismiss();
  }
}
