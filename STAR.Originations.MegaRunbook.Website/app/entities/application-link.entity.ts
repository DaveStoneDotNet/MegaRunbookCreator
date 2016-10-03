
import { ApplicationGroup } from './application-group.entity';
import { ServiceLink }      from './service-link.entity';

export class ApplicationLink {

    constructor(
        public Id:                 number, 
        public ApplicationGroupId: number, 
        public Name:               string,
        public Notes:              string,

        public ServiceLinks:       ServiceLink[]
    ) { }

}