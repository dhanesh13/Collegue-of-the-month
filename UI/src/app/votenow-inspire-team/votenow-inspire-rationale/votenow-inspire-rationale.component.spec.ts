import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VotenowInspireRationaleComponent } from './votenow-inspire-rationale.component';

describe('VotenowInspireRationaleComponent', () => {
  let component: VotenowInspireRationaleComponent;
  let fixture: ComponentFixture<VotenowInspireRationaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VotenowInspireRationaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VotenowInspireRationaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
