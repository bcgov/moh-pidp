import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import {
  faCaretLeft,
  faCaretRight,
  faCircle,
} from '@fortawesome/free-solid-svg-icons';

import { PidpViewport, ViewportService } from '@bcgov/shared/ui';

import { IPortalSection } from '../../state/portal-section.model';

@Component({
  selector: 'app-portal-carousel',
  templateUrl: './portal-carousel.component.html',
  styleUrls: ['./portal-carousel.component.scss'],
})
export class PortalCarouselComponent implements OnInit {
  public cardsPerSlide = 1;

  public faCaretLeft = faCaretLeft;
  public faCaretRight = faCaretRight;
  public faCircle = faCircle;

  @Input() public sections?: IPortalSection[];
  @Input() public portalCategoryName!: string;

  public groups: IPortalSection[][] = [] as IPortalSection[][];

  /**
   * Navigation with arrows is enabled when there is more than one card per slide.
   */
  public get isNavigationEnabled(): boolean {
    return this.cardsPerSlide > 1;
  }

  public constructor(private router: Router, viewportService: ViewportService) {
    viewportService.viewportBroadcast$.subscribe((viewport) => {
      if (viewport === PidpViewport.xsmall) {
        this.cardsPerSlide = 1;
      } else {
        this.cardsPerSlide = 3;
      }
    });
  }

  public ngOnInit(): void {
    this.groups = this.getCarouselGroups(this.sections);
  }
  public onClick(section: IPortalSection): void {
    this.router.navigateByUrl(section.action.route);
  }

  private getCarouselGroups(
    allSections?: IPortalSection[]
  ): IPortalSection[][] {
    // Map state.profile such that each carousel slide gets an array of portal cards.
    const slideModels: IPortalSection[][] = [] as IPortalSection[][];
    if (!allSections?.length) {
      return slideModels;
    }
    let cardModelsForThisSlide: IPortalSection[] = [] as IPortalSection[];
    for (let index = 0; index < allSections.length; index++) {
      const section = allSections[index];
      cardModelsForThisSlide.push(section);
      if (cardModelsForThisSlide.length >= this.cardsPerSlide) {
        // Add this array of cards to the array of slides.
        // NOTE: This array of cards will display as a group in a single slide.
        slideModels.push(cardModelsForThisSlide);

        // Reset the cards array.
        cardModelsForThisSlide = [] as IPortalSection[];
      }
    }
    // If there are any cards left not yet added to a slide, then add them to a new slide.
    if (cardModelsForThisSlide.length > 0) {
      slideModels.push(cardModelsForThisSlide);
    }

    return slideModels;
  }
}
