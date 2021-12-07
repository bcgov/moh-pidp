import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'ui-page-subheader',
  templateUrl: './page-subheader.component.html',
  styleUrls: ['./page-subheader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PageSubheaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
