import { TestBed } from '@angular/core/testing';

import { SavedPostsServiceService } from './saved-posts-service.service';

describe('SavedPostsServiceService', () => {
  let service: SavedPostsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SavedPostsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
