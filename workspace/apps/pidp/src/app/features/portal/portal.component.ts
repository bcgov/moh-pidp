import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DemoService } from '@app/core/services/demo.service';
import { AlertType } from '@bcgov/shared/ui';
import { PartyResource } from '@core/resources/party-resource.service';

import { ShellRoutes } from '../shell/shell.routes';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  description: string;
  process: 'manual' | 'automatic';
  hint?: string;
  actionLabel?: string;
  route?: string;
  statusType?: AlertType;
  status?: string;
  disabled: boolean;
}

// TODO find a clean way to type narrowing in the template
@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public title: string;
  public showCollectionNotice: boolean;
  public state: Record<string, PortalSection[]>;
  public profileComplete: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyResource: PartyResource,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;

    this.showCollectionNotice = this.demoService.showCollectionNotice;
    this.state = this.demoService.state;
    this.profileComplete = this.demoService.profileComplete;
  }

  public onCloseCollectionNotice(disable: boolean): void {
    this.demoService.showCollectionNotice = !disable;
  }

  public onAction(routePath?: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public ngOnInit(): void {
    this.getSummaries();
  }

  public fakeStatusUpdate(type: string): void {
    if (type === 'personal-information') {
      this.state.profileIdentitySections =
        this.state.profileIdentitySections.map((section: PortalSection) => {
          switch (section.type) {
            case 'personal-information':
              return {
                ...section,
                statusType: 'success',
                status: 'completed',
              };
            case 'provider-checklist':
              return {
                ...section,
                disabled: false,
              };
            case 'college-licence-information':
              return {
                ...section,
                disabled: false,
              };
          }

          return section;
        });
      this.state.trainingSections = this.state.trainingSections.map(
        (section: PortalSection) => {
          switch (section.type) {
            case 'compliance-training':
              return {
                ...section,
                disabled: false,
              };
          }

          return section;
        }
      );
    }
    this.state.yourProfileSections = this.state.yourProfileSections.map(
      (section: PortalSection) => ({
        ...section,
        disabled: false,
      })
    );
  }

  private getSummaries(): void {
    this.state;
  }
}
