import {
  Component,
  Input,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';

import { HtmlComponent } from '@bcgov/shared/ui';

import { SharedModule } from '../../../../shared/shared.module';
import { PendingEndorsementComponent } from './components/pending-endorsement/pending-endorsement.component';

export interface IAlertContent {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data: any;
}

@Component({
  selector: 'app-portal-alert',
  standalone: true,
  template: ` <div>
    <header>
      <h2>{{ heading }}</h2>
    </header>
    <div class="section-content-box">
      <ng-template #alertContentHost></ng-template>
    </div>
  </div>`,
  styleUrls: ['./portal-alert.scss'],
  imports: [SharedModule, PendingEndorsementComponent],
})
export class PortalAlertComponent implements OnInit {
  @Input() public heading!: string;
  @Input() public content?: string | Type<unknown>;

  @ViewChild('alertContentHost', { static: true, read: ViewContainerRef })
  public alertContentHost!: ViewContainerRef;

  public ngOnInit(): void {
    if (typeof this.content !== 'string' && this.content) {
      this.loadAlertContentComponent(this.content);
    } else if (this.content) {
      this.loadAlertContentStatic({ content: this.content });
    }
  }

  private loadAlertContentComponent(componentType: Type<unknown>): void {
    this.alertContentHost.createComponent<unknown>(componentType);
  }

  private loadAlertContentStatic(data: unknown): void {
    const componentRef =
      this.alertContentHost.createComponent<unknown>(HtmlComponent);

    const componentInstance = componentRef.instance as IAlertContent;
    componentInstance.data = data;
  }
}
