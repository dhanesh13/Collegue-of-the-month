import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RationaleSparkComponent } from './rationale-spark.component';

describe('RationaleSparkComponent', () => {
  let component: RationaleSparkComponent;
  let fixture: ComponentFixture<RationaleSparkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RationaleSparkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RationaleSparkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
