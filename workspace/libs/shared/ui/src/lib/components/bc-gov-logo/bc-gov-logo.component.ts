import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';

type svgSize = 'small' | 'medium' | 'large';

interface SvgConfig {
  width: string;
  height: string;
  viewbox: string;
}

@Component({
  selector: 'ui-bc-gov-logo',
  templateUrl: './bc-gov-logo.component.svg',
  styleUrls: ['./bc-gov-logo.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class BcGovLogoComponent implements OnInit {
  @Input() public size?: svgSize;
  @Input() public theme?: 'dark' | 'light';

  public config!: SvgConfig;

  private readonly scale: Record<svgSize, number>;

  public constructor() {
    this.scale = {
      small: 0.16,
      medium: 0.26,
      large: 0.36,
    };
    this.theme = 'dark';
  }

  public ngOnInit(): void {
    this.config = {
      ...this.buildConfig(this.size),
    };
  }

  private buildConfig(size: svgSize = 'small'): SvgConfig {
    const viewbox = this.getViewbox();
    const dimensions = this.getDimensions(size, viewbox);
    return {
      ...dimensions,
      viewbox: viewbox.join(','),
    };
  }

  private getViewbox(): number[] {
    // Viewbox top/left padding
    const vbCoords = [0, 0];
    // Dimensions of SVG for proper scaling
    const vbDimensions = [1130, 436];

    return [...vbCoords, ...vbDimensions];
  }

  private getDimensions(
    size: svgSize,
    viewbox: number[],
  ): Pick<SvgConfig, 'width' | 'height'> {
    return {
      width: `${viewbox[2] * this.scale[size]}px`,
      height: `${viewbox[3] * this.scale[size]}px`,
    };
  }
}
