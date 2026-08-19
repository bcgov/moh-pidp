import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
  inject,
} from '@angular/core';

import { PermissionsService } from './permissions.service';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[permittedRoles]',
  standalone: true,
})
export class PermissionsDirective implements OnInit {
  private vcr = inject(ViewContainerRef);
  private tpl = inject(TemplateRef<unknown>);
  private permissionsService = inject(PermissionsService);

  @Input() public permittedRoles!: string | string[];

  public ngOnInit(): void {
    if (this.permissionsService.hasRole(this.permittedRoles)) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }
}
