import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, exhaustMap, of, switchMap } from 'rxjs';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { EnvironmentName } from '../../../../../environments/environment.model';
import {
  AdminResource,
  PartyList,
} from '../../shared/resources/admin-resource.service';

@Component({
  selector: 'app-parties',
  templateUrl: './parties.page.html',
  styleUrls: ['./parties.page.scss'],
})
export class PartiesPage implements OnInit {
  public title: string;
  public dataSource: MatTableDataSource<PartyList>;
  public displayedColumns: string[] = [
    'id',
    'providerName',
    'providerCollegeCode',
    'saEforms',
  ];
  public environment: string;
  public production: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private adminResource: AdminResource,
    private dialog: MatDialog,
    route: ActivatedRoute
  ) {
    this.title = route.snapshot.data.title;
    this.dataSource = new MatTableDataSource();
    this.environment = this.config.environmentName;
    this.production = EnvironmentName.PRODUCTION;
  }

  public onDelete(): void {
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
          result ? this.adminResource.deleteParties() : EMPTY
        ),
        switchMap(() => of(this.getParties()))
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
          (this.dataSource.data = parties.sort((a, b) => a.id - b.id))
      );
  }
}
