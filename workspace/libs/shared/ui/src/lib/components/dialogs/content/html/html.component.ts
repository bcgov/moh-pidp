import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnInit,
} from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'ui-html',
  template: ` <p [innerHtml]="data.message | safe: 'html'"></p> `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HtmlComponent implements OnInit {
  public constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}

  public ngOnInit(): void {}
}
