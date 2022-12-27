import { TestBed } from '@angular/core/testing';

import { SalesDataService } from '../sales-data.service';

describe('SalesDataServiceService', () => {
  let service: SalesDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalesDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
