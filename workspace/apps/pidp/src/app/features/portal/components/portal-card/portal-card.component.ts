import { Component, Inject, Input } from '@angular/core';
import { Router } from '@angular/router';

import { faPlus, faThumbsUp } from '@fortawesome/free-solid-svg-icons';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AuthService } from '@app/features/auth/services/auth.service';

import { IPortalSection } from '../../state/portal-section.model';

@Component({
  selector: 'app-portal-card',
  templateUrl: './portal-card.component.html',
  styleUrls: ['./portal-card.component.scss'],
})
export class PortalCardComponent {
  public faThumbsUp = faThumbsUp;
  public faPlus = faPlus;
  public logoutRedirectUrl: string;

  @Input() public section!: IPortalSection;
  /**
   * Indicates which area of the portal page this section is in.
   */
  @Input() public portalCategoryName!: string;
  /**
   * Index of this card within the category.
   */
  @Input() public isFirst!: boolean;

  public get showCompleted(): boolean {
    const show = this.section.status === 'Completed';
    return show;
  }
  public get showLearnMore(): boolean {
    return (
      !this.showCompleted &&
      !this.section.action.disabled &&
      !this.section.action.openInNewTab
    );
  }
  public get showVisit(): boolean {
    return !!this.section.action.openInNewTab;
  }
  public get hasImageCssClass(): boolean {
    return (
      this.isFirst &&
      (this.isProfileCardCategory ||
        this.isOrganizationCategory ||
        this.isAccessToSystemsCategory)
    );
  }

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private authService: AuthService
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  public onClick(section: IPortalSection): void {
    this.router.navigate(
      [section.action.route],
      section.action.navigationExtras
    );
  }

  public onClickVisit(section: IPortalSection): void {
    window.open(section.action.route, '_blank');
    this.router.navigateByUrl('/');
    this.authService.logout(this.logoutRedirectUrl);
  }

  public get isProfileCardCategory(): boolean {
    return this.portalCategoryName === 'profile';
  }
  public get isOrganizationCategory(): boolean {
    return this.portalCategoryName === 'organization';
  }
  public get isAccessToSystemsCategory(): boolean {
    return this.portalCategoryName === 'access-to-systems';
  }
}
