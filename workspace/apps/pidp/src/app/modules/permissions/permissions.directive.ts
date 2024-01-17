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
  selector: '[permittedRoles]',
  standalone: true,
})
export class PermissionsDirective implements OnInit {
  @Input() public permittedRoles!: string | string[];

  public constructor(
    private vcr: ViewContainerRef,
    private tpl: TemplateRef<unknown>,
    private permissionsService: PermissionsService,
  ) {}

  public ngOnInit(): void {
    if (this.permissionsService.hasRole(this.permittedRoles)) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }
}
