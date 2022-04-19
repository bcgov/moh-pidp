import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { GetSupportComponent } from './get-support.component';

describe('GetSupportComponent', () => {
  let component: GetSupportComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        GetSupportComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    component = TestBed.inject(GetSupportComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
