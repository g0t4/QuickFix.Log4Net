require 'rake'
require 'albacore'

$projectSolution = 'src/QuickFix.Log4Net.sln'
$artifactsPath = "build"
$nugetFeedPath = ENV["NuGetDevFeed"]
$srcPath = File.expand_path('src')

task :teamcity => [:build]

task :build => [:build_release, :tests]

msbuild :build_release => [:clean, :dep] do |msb|
  msb.properties :configuration => :Release
  msb.targets :Build
  msb.solution = $projectSolution
end

task :clean do
    puts "Cleaning"
    FileUtils.rm_rf $artifactsPath
	bins = FileList[File.join($srcPath, "**/bin")].map{|f| File.expand_path(f)}
	bins.each do |file|
		sh %Q{rmdir /S /Q "#{file}"}
    end
end

task :nuget => [:build] do
	sh "nuget pack src\\QuickFix.Log4Net\\QuickFix.Log4Net.csproj /OutputDirectory " + $nugetFeedPath
	sh "nuget pack src\\QuickFixN.Log4Net\\QuickFixN.Log4Net.csproj /OutputDirectory " + $nugetFeedPath
end

desc "Setup dependencies for nuget packages"
task :dep do
	package_folder = File.expand_path('src/packages')
    packages = FileList["**/packages.config"].map{|f| File.expand_path(f)}
	packages.each do |file|
		sh %Q{nuget install #{file} /OutputDirectory #{package_folder}}
    end
end

nunit :tests do |nunit|
	nunit.command = 'src\packages\NUnit.2.5.10.11092\tools\nunit-console-x86.exe'
	$testsPath = 'build'
	nunit.assemblies File.join($testsPath,'QuickFix.Log4Net.Tests.dll')
end
