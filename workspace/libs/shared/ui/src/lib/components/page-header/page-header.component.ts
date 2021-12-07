import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'ui-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PageHeaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
