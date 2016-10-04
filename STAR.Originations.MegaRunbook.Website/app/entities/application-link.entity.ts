
import { ApplicationGroup } from './application-group.entity';
import { ApplicationType }  from './application-type.entity';
import { ServiceLink }      from './service-link.entity';

export class ApplicationLink {

    constructor(
        public Id:                 number, 
        public ApplicationGroupId: number, 
        public Name:               string,
        public Notes:              string,
        public IsSelected:         boolean,

        public ApplicationType:    ApplicationType[], 
        public ServiceLinks:       ServiceLink[]
    ) { }

}