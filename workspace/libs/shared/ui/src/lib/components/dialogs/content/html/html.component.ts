import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { IDialogContent } from '@bcgov/shared/ui';

@Component({
  selector: 'ui-html',
  template: ` <p [innerHtml]="data.content | safe: 'html'"></p> `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HtmlComponent implements IDialogContent {
  @Input() public data!: { content: string };
}
