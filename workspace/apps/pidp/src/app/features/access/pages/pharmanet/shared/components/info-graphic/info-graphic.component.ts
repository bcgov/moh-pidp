import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-info-graphic',
  templateUrl: './info-graphic.component.html',
  styleUrls: ['./info-graphic.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InfoGraphicComponent {
  @Input() public infoGraphicName!:
    | 'approval'
    | 'notification'
    | 'moa'
    | 'global';
}
