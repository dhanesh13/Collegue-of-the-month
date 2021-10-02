import { TestBed } from '@angular/core/testing';

import { RationaleService } from './rationale.service';

describe('RationaleService', () => {
  let service: RationaleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RationaleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
