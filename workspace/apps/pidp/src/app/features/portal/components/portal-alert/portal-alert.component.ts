import {
  Component,
  Input,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';

import { HtmlComponent } from '@bcgov/shared/ui';

@Component({
  selector: 'app-portal-alert',
  template: ` <div>
    <header>
      <h2>{{ heading }}</h2>
    </header>
    <div class="section-content-box">
      <ng-template #alertContentHost></ng-template>
    </div>
  </div>`,
  styleUrls: ['./portal-alert.scss'],
  standalone: true,
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
      this.loadAlertContentStatic(this.content);
    }
  }

  private loadAlertContentComponent(componentType: Type<unknown>): void {
    this.alertContentHost.createComponent<unknown>(componentType);
  }

  private loadAlertContentStatic(content: string): void {
    const componentRef =
      this.alertContentHost.createComponent<HtmlComponent>(HtmlComponent);
    componentRef.instance.data = { content };
  }
}
