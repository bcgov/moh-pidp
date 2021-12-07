import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public showCollectionNotice: boolean;

  public constructor() {
    this.showCollectionNotice = true;
  }

  public ngOnInit(): void {}
}
