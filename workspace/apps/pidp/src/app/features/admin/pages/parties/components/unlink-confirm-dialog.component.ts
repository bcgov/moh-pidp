import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { IDialogContent } from '@bcgov/shared/ui';

@Component({
  selector: 'app-unlink-confirm-dialog',
  template: `
    <p>{{ data.message }}</p>
    <mat-checkbox [(ngModel)]="deleteFromBcProvider" (change)="onChange()">
      Also delete the user account from the BC Provider directory (Microsoft Entra)
    </mat-checkbox>
  `,
  styles: [
    `
      mat-checkbox {
        margin-top: 1rem;
        display: block;
      }
    `,
  ],
  imports: [MatCheckboxModule, FormsModule],
})
export class UnlinkConfirmDialogComponent implements IDialogContent {
  @Input() public data!: { message: string };
  @Output() public output = new EventEmitter<{ deleteFromBcProvider: boolean }>();

  public deleteFromBcProvider = false;

  public onChange(): void {
    this.output.emit({ deleteFromBcProvider: this.deleteFromBcProvider });
  }
}
