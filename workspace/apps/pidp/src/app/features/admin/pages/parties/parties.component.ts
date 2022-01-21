import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

import {
  AdminResource,
  PartyList,
} from '../../shared/resources/admin-resource.service';

@Component({
  selector: 'app-parties',
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.scss'],
})
export class PartiesComponent implements OnInit {
  public title: string;
  public dataSource: MatTableDataSource<PartyList>;
  public displayedColumns: string[] = [
    'id',
    'providerName',
    'providerCollegeCode',
    'saEforms',
  ];

  public constructor(
    private adminResource: AdminResource,
    route: ActivatedRoute
  ) {
    this.title = route.snapshot.data.title;
    this.dataSource = new MatTableDataSource();
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
