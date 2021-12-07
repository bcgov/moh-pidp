import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

export interface DashboardHeaderConfig {
  theme?: 'blue' | 'white';
  // TODO change name to reflect a default that will show the toggle
  showMobileToggle?: boolean;
}

@Component({
  selector: 'app-dashboard-header',
  templateUrl: './dashboard-header.component.html',
  styleUrls: ['./dashboard-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardHeaderComponent {
  /**
   * @description
   * Dashboard header configuration for theming.
   */
  @Input() public headerConfig: DashboardHeaderConfig;
  /**
   * @description
   * Username displayed in the dashboard header.
   */
  @Input() public username!: string;
  /**
   * @description
   * Indicator that viewport dimensions match a
   * mobile device.
   *
   * NOTE: showMobileToggle will be displayed when the
   * mobile device viewport dimension are provided.
   */
  @Input() public isMobile!: boolean;
  /**
   * @description
   * Event emission of toggling the dashboard sidenav
   * drawer action.
   */
  @Output() public toggle: EventEmitter<void>;
  /**
   * @description
   * Event emission of logout action.
   */
  @Output() public logout: EventEmitter<void>;

  public brandImageSrc: string;

  public constructor() {
    this.headerConfig = {
      theme: 'blue',
      showMobileToggle: true,
    };
    this.toggle = new EventEmitter<void>();
    this.logout = new EventEmitter<void>();

    this.brandImageSrc =
      this.headerConfig.theme === 'white'
        ? '/assets/images/gov_bc_logo_white.jpeg'
        : '/assets/images/gov_bc_logo_blue.png';
  }

  public toggleSidenav(): void {
    this.toggle.emit();
  }

  public onLogout(): void {
    this.logout.emit();
  }
}
