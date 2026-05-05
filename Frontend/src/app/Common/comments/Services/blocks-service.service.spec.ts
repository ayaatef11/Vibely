import { TestBed } from '@angular/core/testing';

import { BlocksServiceService } from './blocks-service.service';

describe('BlocksServiceService', () => {
  let service: BlocksServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BlocksServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
