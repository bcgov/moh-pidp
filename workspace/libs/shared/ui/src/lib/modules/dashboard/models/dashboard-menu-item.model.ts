import { IsActiveMatchOptions, NavigationExtras } from '@angular/router';

import { RoutePath } from '@bcgov/shared/utils';

export type DashboardMenuType = 'route' | 'list';

export interface DashboardMenuDetail {
  readonly type: DashboardMenuType;
}

export interface DashboardMenuItemOptions {
  deemphasize?: boolean;
  disabled?: boolean;
}

export abstract class DashboardMenuItem {
  public label: string;
  public icon: string;
  public options: DashboardMenuItemOptions;

  public constructor(
    label: string,
    icon = '',
    options: DashboardMenuItemOptions = {},
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
   * Options to configure how to determine if the
   * router link is active.
   */
  public linkActiveOptions:
    | {
        exact: boolean;
      }
    | IsActiveMatchOptions;

  public constructor(
    label: string,
    routeParams: {
      commands: RoutePath;
      extras?: NavigationExtras;
      linkActiveOptions?:
        | {
            exact: boolean;
          }
        | IsActiveMatchOptions;
    },
    icon = '',
    menuOptions: DashboardMenuItemOptions = {},
  ) {
    super(label, icon, menuOptions);

    const { commands, extras, linkActiveOptions } = routeParams;

    this.type = 'route';
    this.commands = commands;
    this.extras = extras ?? {};
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
    icon = '',
    options: DashboardMenuItemOptions = {},
  ) {
    super(label, icon, options);

    this.type = 'list';
    this.children = children;
  }
}
