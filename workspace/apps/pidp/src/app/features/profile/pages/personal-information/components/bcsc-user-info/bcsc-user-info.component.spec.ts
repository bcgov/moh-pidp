import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BcscUserInfoComponent } from './bcsc-user-info.component';

describe('BcscUserInfoComponent', () => {
  let component: BcscUserInfoComponent;
  let fixture: ComponentFixture<BcscUserInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BcscUserInfoComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BcscUserInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
