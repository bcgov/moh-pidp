import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { ConfirmDialogComponent } from '@bcgov/shared/ui';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  public constructor(private dialog: MatDialog) {}

  public canDeactivateFormDialog(): MatDialogRef<ConfirmDialogComponent> {
    const data = 'unsaved';
    return this.dialog.open(ConfirmDialogComponent, { data });
  }
}
