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
  private readonly duration: number;
  private readonly horizontalPosition: MatSnackBarHorizontalPosition;
  private readonly verticalPosition: MatSnackBarVerticalPosition;

  public constructor(private snackBar: MatSnackBar) {
    this.duration = 3000; // ms
    this.verticalPosition = 'top';
    this.horizontalPosition = 'end';
  }

  /**
   * @description
   * Opens a toast to display a success message.
   */
  public openSuccessToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig
  ): void {
    const defaultConfig: MatSnackBarConfig = Object.assign(
      {
        duration: this.duration,
        extraClasses: ['success'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      },
      config
    );
    this.openToast(message, action, defaultConfig);
  }

  /**
   * @description
   * Opens a toast to display an error message.
   */
  public openErrorToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig
  ): void {
    const defaultConfig: MatSnackBarConfig = Object.assign(
      {
        duration: this.duration,
        extraClasses: ['danger'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      },
      config
    );
    this.openToast(message, action, defaultConfig);
  }

  private openToast(
    message: string,
    action?: string,
    config?: MatSnackBarConfig
  ): void {
    this.snackBar.open(message, action, config);
  }
}
