import { NavigationExtras } from '@angular/router';

import { RoutePath } from '@bcgov/shared/utils';

export type DashboardMenuType = 'route' | 'list';

export interface DashboardMenuDetail {
  readonly type: DashboardMenuType;
}

export interface DashboardMenuItemOptions {
  active?: boolean;
  deemphasize?: boolean;
  disabled?: boolean;
}

export abstract class DashboardMenuItem {
  public label: string;
  public icon: string;
  public options: DashboardMenuItemOptions;

  public constructor(
    label: string,
    icon: string = '',
    options: DashboardMenuItemOptions = {}
  ) {
    this.label = label;
    this.icon = icon;
    this.options = options;
  }

  /**
   * @description
   * Current state of the menu item options.
   */
  public getOptions(): DashboardMenuItemOptions {
    return {
      active: this.options?.active,
      disabled: this.options?.disabled,
      deemphasize: this.options?.deemphasize,
    };
  }
}

export class DashboardRouteMenuItem
  extends DashboardMenuItem
  implements DashboardMenuDetail
{
  public readonly type: DashboardMenuType;
  /**
   * @description
   * URL fragments with which to construct the target URL.
   */
  public commands: RoutePath;
  /**
   * @description
   * Options that modify the route URL.
   */
  public extras: NavigationExtras;
  /**
   * @description
   * CSS class applied
   */
  public linkActiveClass: string | string[];

  public linkActiveOptions: { exact: boolean };

  public constructor(
    label: string,
    routeParams: {
      commands: RoutePath;
      extras?: NavigationExtras;
      linkActiveClass?: string | string[];
      linkActiveOptions?: { exact: boolean };
    },
    icon: string = '',
    // TODO what to do with options?
    menuOptions: DashboardMenuItemOptions = {}
  ) {
    super(label, icon, menuOptions);

    const { commands, extras, linkActiveClass, linkActiveOptions } =
      routeParams;

    this.type = 'route';
    this.commands = commands;
    this.extras = extras ?? {};
    this.linkActiveClass = linkActiveClass ?? 'ng-active';
    this.linkActiveOptions = linkActiveOptions ?? { exact: true };
  }
}

export class DashboardListMenuItem
  extends DashboardMenuItem
  implements DashboardMenuDetail
{
  public readonly type: DashboardMenuType;
  public children: DashboardMenuItem[];

  public constructor(
    label: string,
    children: DashboardMenuItem[],
    icon: string = '',
    options: DashboardMenuItemOptions = {}
  ) {
    super(label, icon, options);

    this.type = 'list';
    this.children = children;
  }
}
