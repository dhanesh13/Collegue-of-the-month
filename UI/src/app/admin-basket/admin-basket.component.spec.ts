import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBasketComponent } from './admin-basket.component';

describe('AdminBasketComponent', () => {
  let component: AdminBasketComponent;
  let fixture: ComponentFixture<AdminBasketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminBasketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminBasketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
