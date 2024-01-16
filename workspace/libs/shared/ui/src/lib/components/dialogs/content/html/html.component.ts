import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { IDialogContent } from '../../dialog-content.model';
import { SafePipe } from '../../../../pipes/safe.pipe';

@Component({
    selector: 'ui-html',
    template: `<p [innerHtml]="data.content | safe: 'html'"></p>`,
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [SafePipe],
})
export class HtmlComponent implements IDialogContent {
  @Input() public data!: { content: string };
}
