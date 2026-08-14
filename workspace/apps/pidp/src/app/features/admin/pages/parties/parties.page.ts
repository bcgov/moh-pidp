
import { Component, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, exhaustMap, of, switchMap } from 'rxjs';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  PageComponent,
  PageHeaderComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { EnvironmentName } from '../../../../../environments/environment.model';
import { LookupCodePipe } from '../../../../modules/lookup/lookup-code.pipe';
import {
  AdminResource,
  PartyList,
} from '../../shared/resources/admin-resource.service';
import { UnlinkConfirmDialogComponent } from './components/unlink-confirm-dialog.component';

@Component({
  selector: 'app-parties',
  templateUrl: './parties.page.html',
  styleUrls: ['./parties.page.scss'],
  imports: [
    LookupCodePipe,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    PageComponent,
    PageHeaderComponent
],
})
export class PartiesPage implements OnInit {
  private readonly config = inject<AppConfig>(APP_CONFIG);
  private readonly adminResource = inject(AdminResource);
  private readonly dialog = inject(MatDialog);

  public title: string;
  public dataSource: MatTableDataSource<PartyList>;
  public displayedColumns: string[] = [
    'id',
    'providerName',
    'providerCollegeCode',
    'saEforms',
    'credentials',
    'delete',
  ];
  public environment: string;
  public production: string;

  public constructor() {
    const route = inject(ActivatedRoute);

    this.title = route.snapshot.data.title;
    this.dataSource = new MatTableDataSource();
    this.environment = this.config.environmentName;
    this.production = EnvironmentName.PRODUCTION;

    if (this.environment === this.production) {
      this.displayedColumns = this.displayedColumns.filter(c => c !== 'delete');
    }
  }

  public onDeleteParty(partyId: number): void {
    this.adminResource
      .deleteParty(partyId)
      .pipe(switchMap(() => of(this.getParties())))
      .subscribe();
  }

  public onUnlinkCredential(partyId: number, credential: any): void {
    if (credential.identityProvider === 'bcprovider') {
      const data: DialogOptions = {
        title: 'Disconnect Credential',
        component: UnlinkConfirmDialogComponent,
        data: {
          message: `Are you sure you want to disconnect the BC Provider credential (${credential.idpId})?`
        }
      };
      this.dialog
        .open(ConfirmDialogComponent, { data })
        .afterClosed()
        .pipe(
          exhaustMap((result) => {
            if (result) {
              const deleteFromBcProvider = !!result.output?.deleteFromBcProvider;
              return this.adminResource.deleteCredential(partyId, credential.id, deleteFromBcProvider);
            }
            return EMPTY;
          }),
          switchMap(() => of(this.getParties())),
        )
        .subscribe();
    } else {
      const data: DialogOptions = {
        title: 'Disconnect Credential',
        component: HtmlComponent,
        data: {
          content: `Are you sure you want to disconnect the ${credential.identityProvider} credential?`
        }
      };
      this.dialog
        .open(ConfirmDialogComponent, { data })
        .afterClosed()
        .pipe(
          exhaustMap((result) =>
            result ? this.adminResource.deleteCredential(partyId, credential.id, false) : EMPTY,
          ),
          switchMap(() => of(this.getParties())),
        )
        .subscribe();
    }
  }

  public onDeleteParties(): void {
    const data: DialogOptions = {
      title: 'Delete all parties',
      component: HtmlComponent,
      data: {
        content: 'You are about to delete all parties. Continue?',
      },
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result) =>
          result ? this.adminResource.deleteParties() : EMPTY,
        ),
        switchMap(() => of(this.getParties())),
      )
      .subscribe();
  }

  public ngOnInit(): void {
    this.getParties();
  }

  private getParties(): void {
    this.adminResource
      .getParties()
      .subscribe(
        (parties: PartyList[]) =>
          (this.dataSource.data = parties.sort((a, b) => a.id - b.id)),
      );
  }
}
