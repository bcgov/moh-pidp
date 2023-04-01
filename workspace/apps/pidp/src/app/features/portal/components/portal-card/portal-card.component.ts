import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { faPlus, faThumbsUp } from '@fortawesome/free-solid-svg-icons';

import { IPortalSection } from '../../state/portal-section.model';

@Component({
  selector: 'app-portal-card',
  templateUrl: './portal-card.component.html',
  styleUrls: ['./portal-card.component.scss'],
})
export class PortalCardComponent {
  public faThumbsUp = faThumbsUp;
  public faPlus = faPlus;

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
    return !this.showCompleted && !this.section.action.disabled;
  }
  public get hasImageCssClass(): boolean {
    return (
      this.isFirst &&
      (this.isProfileCardCategory ||
        this.isOrganizationCategory ||
        this.isAccessToSystemsCategory)
    );
  }

  public constructor(private router: Router) {}

  public onClick(section: IPortalSection): void {
    this.router.navigateByUrl(section.action.route);
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
