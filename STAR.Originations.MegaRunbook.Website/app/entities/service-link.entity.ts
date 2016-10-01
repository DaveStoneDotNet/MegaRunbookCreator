
import { Link } from './link.entity';

export class ServiceLink {

    constructor(
        public ID: number, 
        public FolderName: string,
        public ServiceName: string,
        public Notes: string,
        public ServiceLinks: Link[],
        public FolderLinks: Link[]
    ) { }

}