import { TestBed } from '@angular/core/testing';

import { InfoGraphicComponent } from './info-graphic.component';

describe('InfoGraphicComponent', () => {
  let component: InfoGraphicComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InfoGraphicComponent],
    });

    component = TestBed.inject(InfoGraphicComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
