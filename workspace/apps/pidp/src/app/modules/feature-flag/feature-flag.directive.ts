import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';

import { FeatureFlagService } from './feature-flag.service';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[featureFlag]',
})
export class FeatureFlagDirective implements OnInit {
  @Input() public featureFlag!: string | string[];
  @Input() public featureFlagOr: string;

  public constructor(
    private vcr: ViewContainerRef,
    private tpl: TemplateRef<unknown>,
    private featureFlagService: FeatureFlagService
  ) {
    this.featureFlagOr = '';
  }

  public ngOnInit(): void {
    if (this.featureFlagService.hasFlag(this.featureFlag)) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }
}
