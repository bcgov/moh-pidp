import { NgIf } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-parties',
  templateUrl: './parties.page.html',
  styleUrls: ['./parties.page.scss'],
  standalone: true,
  imports: [
    LookupCodePipe,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    NgIf,
    PageComponent,
    PageHeaderComponent,
  ],
})
export class PartiesPage implements OnInit {
  public title: string;
  public dataSource: MatTableDataSource<PartyList>;
  public displayedColumns: string[] = [
    'id',
    'providerName',
    'providerCollegeCode',
    'saEforms',
    'delete',
  ];
  public environment: string;
  public production: string;

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly adminResource: AdminResource,
    private readonly dialog: MatDialog,
    route: ActivatedRoute,
  ) {
    this.title = route.snapshot.data.title;
    this.dataSource = new MatTableDataSource();
    this.environment = this.config.environmentName;
    this.production = EnvironmentName.PRODUCTION;
  }

  public onDeleteParty(partyId: number): void {
    this.adminResource
      .deleteParty(partyId)
      .pipe(switchMap(() => of(this.getParties())))
      .subscribe();
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
