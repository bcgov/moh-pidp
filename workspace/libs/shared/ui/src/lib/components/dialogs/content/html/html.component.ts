import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { DialogOptions } from '../../dialog-options.model';

@Component({
  selector: 'ui-html',
  template: `<p [innerHtml]="data?.message | safe: 'html'"></p>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HtmlComponent {
  public constructor(@Inject(MAT_DIALOG_DATA) public data: DialogOptions) {}
}
