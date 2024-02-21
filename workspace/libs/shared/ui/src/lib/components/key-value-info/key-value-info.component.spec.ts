import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyValueInfoComponent } from './key-value-info.component';

describe('KeyValueInfoComponent', () => {
  let component: KeyValueInfoComponent;
  let fixture: ComponentFixture<KeyValueInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({}).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyValueInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
