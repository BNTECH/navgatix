const fs = require('fs');
const path = require('path');

const targetPath = path.join('c:', 'navneet_code_folder', 'navgatix', 'navgatix', 'ClientApp', 'src', 'pages', 'transporter', 'TransporterDashboard.tsx');

let content = fs.readFileSync(targetPath, 'utf8');

content = content.replace(
`    vehicleNumber?: string;
    vehicleName?: string;
    vehicleTypeName?: string;
    driverName?: string;`,
`    vehicleNumber?: string;
    vehicleName?: string;
    vehicleTypeName?: string;
    driverId?: string;
    driverName?: string;`
);

content = content.replace(
`                    vehicleName: item.vehicleName ?? item.VehicleName,
                    vehicleTypeName: item.vehicleTypeName ?? item.VehicleTypeName,
                    driverName: item.driverName ?? item.DriverName,`,
`                    vehicleName: item.vehicleName ?? item.VehicleName,
                    vehicleTypeName: item.vehicleTypeName ?? item.VehicleTypeName,
                    driverId: item.driverId ?? item.DriverId,
                    driverName: item.driverName ?? item.DriverName,`
);

content = content.replace(
`                        <button
                            onClick={() => setActiveTab('vehicles')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'vehicles' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Truck className={\`h-5 w-5 \${activeTab === 'vehicles' ? 'text-primary-600' : ''}\`} />
                            Vehicle Fleet
                        </button>
                        <button className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-slate-900 transition-all duration-200">
                            <FileText className="h-5 w-5" />
                            Reports
                        </button>`,
`                        <button
                            onClick={() => setActiveTab('vehicles')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'vehicles' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Truck className={\`h-5 w-5 \${activeTab === 'vehicles' ? 'text-primary-600' : ''}\`} />
                            Vehicle Fleet
                        </button>
                        <button
                            onClick={() => setActiveTab('requests')}
                            className={\`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 \${activeTab === 'requests' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}\`}
                        >
                            <Package className={\`h-5 w-5 \${activeTab === 'requests' ? 'text-primary-600' : ''}\`} />
                            Ride Requests
                        </button>
                        <button className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-slate-900 transition-all duration-200">
                            <FileText className="h-5 w-5" />
                            Reports
                        </button>`
);

content = content.replace(
`                        </div>
                    </div>
                </div>
                    )}`,
`                        </div>
                    </div>
                    )}`
);

fs.writeFileSync(targetPath, content, 'utf8');
console.log('done node fix');
