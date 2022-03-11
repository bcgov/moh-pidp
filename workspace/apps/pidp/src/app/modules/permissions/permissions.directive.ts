import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';

import { PermissionsService } from './permissions.service';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[permissions]',
})
export class PermissionsDirective implements OnInit {
  @Input() public roles!: string | string[];
  @Input() public roleOr: string;

  public constructor(
    private vcr: ViewContainerRef,
    private tpl: TemplateRef<unknown>,
    private permissionsService: PermissionsService
  ) {
    this.roleOr = '';
  }

  public ngOnInit(): void {
    if (
      this.permissionsService.hasRole(this.roles) ||
      this.permissionsService.hasRole(this.roleOr)
    ) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }
}
