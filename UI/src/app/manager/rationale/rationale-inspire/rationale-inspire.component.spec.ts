import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RationaleInspireComponent } from './rationale-inspire.component';

describe('RationaleInspireComponent', () => {
  let component: RationaleInspireComponent;
  let fixture: ComponentFixture<RationaleInspireComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RationaleInspireComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RationaleInspireComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
