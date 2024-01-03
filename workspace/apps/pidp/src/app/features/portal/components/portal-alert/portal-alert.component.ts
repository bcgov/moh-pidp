import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';

import { SharedModule } from '../../../../shared/shared.module';

export interface IAlertContent {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data: any;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  output?: EventEmitter<any>;
}

export interface AlertContentOutput<T> {
  output: T;
}

export type ComponentType<T = unknown> = Type<T>;

@Component({
  selector: 'app-portal-alert',
  standalone: true,
  template: ` <div>
    <header>
      <h2>{{ heading }}</h2>
    </header>
    <div class="section-content-box">
      <!-- <p [innerHtml]="content | safe: 'html'" (click)="onClickAlert()"></p> -->
      <ng-template #alertContentHost></ng-template>
    </div>
  </div>`,
  styleUrls: ['./portal-alert.scss'],
  imports: [CommonModule, SharedModule],
})
export class PortalAlertComponent implements OnInit {
  public alertContentOutput: AlertContentOutput<unknown> | null;
  @Input() public heading!: string;
  @Input() public content?: string | Type<unknown>;
  @Input() public route?: string;
  // @Output() public alertClicked = new EventEmitter<string>();

  @ViewChild('alertContentHost', { static: true, read: ViewContainerRef })
  public alertContentHost!: ViewContainerRef;

  public constructor() {
    this.alertContentOutput = null;
  }

  // public onClickAlert(): void {
  //   if (!this.route) {
  //     return;
  //   } else {
  //     this.alertClicked.emit(this.route);
  //   }
  // }

  public ngOnInit(): void {
    if (typeof this.content !== 'string' && this.content) {
      this.loadAlertContentComponent(this.content);
    }
  }

  private loadAlertContentComponent(componentType: Type<unknown>): void {
    const componentRef =
      this.alertContentHost.createComponent<unknown>(componentType);
    const componentInstance = componentRef.instance as IAlertContent;
    const output$ = componentInstance.output;

    if (output$) {
      output$.subscribe(
        (value: AlertContentOutput<unknown> | null) =>
          (this.alertContentOutput = value),
      );
    }
  }
}
